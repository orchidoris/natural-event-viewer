
import * as React from 'react';
import { useState, useEffect } from 'react';
import { withRouter } from 'react-router';
import { History } from 'history';
import { connect } from 'react-redux';
import { bindActionCreators, Dispatch } from 'redux';
import { ApplicationState } from '../../../store';
import { SourcesStaticData } from '../../../shared/staticData/sources';

import { fetchEventListEffect } from '../../eventListActions';
import * as styles from './EventListScreen.module.less';
import { EonetEvent, EonetCategory, EonetEventStatus, EonetGeometryType, EonetGeometryPoint } from '../../../shared/models/EonetEvent';
import { EonetEventsRequest, EonetEventOrderAttributeType, EonetEventOrderAttribute, EonetFilters } from '../../models/EventListScreen';
import LeftPanel from '../../../shared/components/LeftPanel/LeftPanel';
import Button from '../../../shared/components/Button/Button';
import Checkbox from '../../../shared/components/Checkbox/Checkbox';
import { List } from '../../../shared/components/List/List';
import { Link } from 'react-router-dom';
import { SearchInput } from '../../../shared/components/SearchInput/SearchInput';
import Input from '../../../shared/components/Input/Input';
import Scrollbars from 'react-custom-scrollbars';

const mapStateToProps = (state: ApplicationState) => ({
    eventListScreenState: state.eventListScreen,
    allSources: state.global.sources,
    allCategories: state.global.categories
});

const mapDispatchToProps = (dispatch: Dispatch) =>
    bindActionCreators(
        {
            fetchEventListEffect
        },
        dispatch
    );

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(EventListScreen) as any);

type StoreProps = ReturnType<typeof mapDispatchToProps> & ReturnType<typeof mapStateToProps>;

export type Props = StoreProps & {
    history: History;
};

export function EventListScreen(p: Props) {
    const maxDaysPriorFilterValue = 180; // TODO: pass this value into global state from backend instead of hardcoding it

    const eventsResponse = p.eventListScreenState.eventsResponse;



    const sourceNameDictionary = Object.assign({}, ...SourcesStaticData.sources.map((s: any) => ({[s.id]: s.title})));

    const [filters, setFilters] = useState<EonetFilters>(p.eventListScreenState.filters);
    const [search, setSearch] = useState(filters.titleSearch);

    useEffect(() => {
        if (eventsResponse.title == 'Initial') {
            p.fetchEventListEffect({}, filters);
        }
    }, []);
    
    function manageFiltersStatuses(status: EonetEventStatus) {
        let newStatuses: EonetEventStatus[] = [...filters.statuses];
    
        if (newStatuses.includes(status)) {
            newStatuses.splice(newStatuses.indexOf(status), 1);
        } else {
            newStatuses = [...newStatuses, status];
        }

        return setFilters({ ...filters, statuses: newStatuses });
    }

    function manageFiltersSource(source: string) {
        let newSources: string[] = [...filters.sources];
    
        if (newSources.includes(source)) {
            newSources.splice(newSources.indexOf(source), 1);
        } else {
            newSources = [...newSources, source];
        }

        return setFilters({ ...filters, sources: newSources });
    }

    function manageFiltersCategories(category: EonetCategory) {
        let newCategories: EonetCategory[] = [...filters.categories];
    
        if (newCategories.includes(category)) {
            newCategories.splice(newCategories.indexOf(category), 1);
        } else {
            newCategories = [...newCategories, category];
        }

        return setFilters({ ...filters, categories: newCategories });
    }

    function manageOrderAttributes(attributeType : EonetEventOrderAttributeType) {
        let newOrder: EonetEventOrderAttributeType[] = [...filters.order];
    
        if (newOrder.includes(attributeType)) {
            newOrder.splice(newOrder.indexOf(attributeType), 1);
        } else {
            newOrder = [...newOrder, attributeType];
        }

        return setFilters({ ...filters, order: newOrder });
    }

    function manageOrderAttributesDirection(attributeType : EonetEventOrderAttributeType) {
        let newAttrDirection: EonetEventOrderAttribute[] = [...filters.orderAttributesDirection].map(ad => ({
            attributeType: ad.attributeType,
            isDescending: ad.attributeType == attributeType ? !ad.isDescending : !!ad.isDescending
        }));

        return setFilters({ ...filters, orderAttributesDirection: newAttrDirection });
    }

    function manageDaysPrior(e: React.ChangeEvent<HTMLInputElement>) {
        const value = e.target.value as string;
        let newDaysPrior = parseInt(value) || 1;

        // TODO: Add warning pop-up for value overflow case
        newDaysPrior = (newDaysPrior > maxDaysPriorFilterValue || newDaysPrior < 1) ? maxDaysPriorFilterValue : newDaysPrior;
        
        if (value !== '' && value !== newDaysPrior.toString()) {
            e.target.value = String(newDaysPrior);
        }

        return setFilters({ ...filters, daysPrior: newDaysPrior });
    };

    function onRequest() {
        const request: EonetEventsRequest =  {
            days: filters.daysPrior,
            sources: filters.sources,
            status: filters.statuses.length == 1 ? filters.statuses[0] : null,
            categories: filters.categories.map(c => c.id),
            titleSearch: search,
            ordering: filters.order.map(attrType => ({
                attributeType: attrType,
                isDescending: filters.orderAttributesDirection.some(oa => oa.attributeType === attrType && oa.isDescending)
            }))
        }

        p.fetchEventListEffect(request, filters);
    }

    function highlightText(text: string) {
        const responseSearchText = eventsResponse.titleSearch;
        if (responseSearchText == '') {
            return text;
        }

        let parts = text.split(new RegExp(`(${responseSearchText})`, 'i'));
        return (
            <span>
                {parts.map((part, i) => (
                    <span key={i} className={`${part.toLowerCase() === responseSearchText.toLowerCase() ? styles.highlight : ''}`}>
                        {part}
                    </span>
                ))}
            </span>
        );
    }

    function getCategoryIconImgClass(categoryId: string) {
        switch(categoryId) {
            case 'volcanoes': return styles.volcanoesCategoryImg;
            case 'landslides': return styles.landslidesCategoryImg;
            case 'severeStorms': return styles.severeStormsCategoryImg;
            case 'wildfires': return styles.wildfiresCategoryImg;
            case 'seaLakeIce': return styles.waterIceCategoryImg;
        }

        return styles.defaultCategoryImg;
    }

    return (
        <div className={styles.container}>
            <LeftPanel> <Scrollbars>
                <SearchInput onChange={setSearch} defaultValue={filters.titleSearch} placeholder="Search by name"></SearchInput>
                <div className={styles.boldText + ' ' + styles.daysPriorLabel}>Days prior:</div>
                    <Input
                        className={styles.daysPriorInput}
                        type="number"
                        name="days-prior"
                        min={1}
                        max={maxDaysPriorFilterValue}
                        onChange={manageDaysPrior}
                        value={filters.daysPrior}
                    />
                <div className={styles.boldText}>Filter by status:</div>
                {Object.values(EonetEventStatus).map((c : EonetEventStatus) => (
                    <div className={styles.searchTypeTextTwoColumns} key={c} title={c}>
                        <Checkbox checked={filters.statuses.includes(c)} label={c} onChange={() => manageFiltersStatuses(c)} />
                    </div>
                ))}
                <div className={styles.boldText}>Filter by category:</div>
                {p.allCategories.map((c : EonetCategory) => (
                    <div className={styles.searchTypeText} key={c.id} title={c.title}>
                        <Checkbox checked={filters.categories.includes(c)} label={c.title} onChange={() => manageFiltersCategories(c)} />
                    </div>
                ))}
                <div className={styles.boldText}>Filter by source:</div>
                {p.allSources.map((s) => (
                    <div className={styles.searchTypeTextTwoColumns} key={s} title={sourceNameDictionary[s]}>
                        <Checkbox checked={filters.sources.includes(s)} label={s} onChange={() => manageFiltersSource(s)} />
                    </div>
                ))}
                <div className={styles.boldText}>Order by:</div>
                {Object.values(EonetEventOrderAttributeType).map((attributeType : EonetEventOrderAttributeType) => (
                    <div className={styles.searchTypeText} key={attributeType} title={attributeType}>
                        <div className={filters.order.indexOf(attributeType) == -1 ? styles.orderNumberNone : styles.orderNumber}>{filters.order.indexOf(attributeType) + 1}</div>
                        <Checkbox className={styles.orderAttr} checked={filters.order.includes(attributeType)}
                            label={attributeType == EonetEventOrderAttributeType.LastDate ? 'Last Date' : attributeType}
                            onChange={() => manageOrderAttributes(attributeType)} />
                        <Checkbox checked={filters.orderAttributesDirection.some(oa => oa.attributeType === attributeType && oa.isDescending)}
                            label="Desc"
                            onChange={() => manageOrderAttributesDirection(attributeType)} />
                    </div>
                ))}
                <Button isPrimary onClick={onRequest}>Request Natural Events</Button>
            </Scrollbars> </LeftPanel>
            <div className={styles.screen}>
                <div className={styles.header}>
                    <div>
                        <span className={`${styles.eventViewerText} ${styles.headerText}`}>Earth Observatory Natural Events Tracker</span>
                        <div className={styles.rectangle} />
                        <span className={styles.headerText}>Events List</span>
                    </div>
                </div>
                <List className={styles.list}>
                    {eventsResponse.events.map((ee: EonetEvent) => (
                        <div key={ee.id} data-event-category={ee.categories[0].id} className={styles.listItem}>
                            <Link to={`/event/${ee.id}`} className={styles.listItemLink}>
                                <div className={styles.categoryBlock}>
                                    <div className={getCategoryIconImgClass(ee.categories[0].id) + (ee.closed ? ' ' + styles.categoryClosed : '')}/>
                                    <div className={styles.categoryName}>{ee.categories[0].title}</div>
                                </div>
                                <div className={styles.eventDetails}>
                                    <div className={styles.id}>{ee.id}</div>
                                    <div className={styles.name}>{eventsResponse.titleSearch ? highlightText(ee.title) : ee.title}</div>
                                    {ee.description ? <div className={styles.listItemDetailsDescription}>{ee.description}</div> : ''}
                                    {(ee.geometry[0] ?? {}).type == EonetGeometryType.Point ?
                                        <div className={styles.locationBlock}>
                                            <div className={styles.locationImg} />
                                            <div className={styles.locationNum}>{(ee.geometry[0] as EonetGeometryPoint).coordinates.join(', ')}</div>
                                        </div>: ''}
                                    <div className={styles.sourceBlock}>
                                        <span className={styles.sourceText}>Sources:</span>
                                        {ee.sources.map((s) => 
                                            <span key={s.id} className={styles.sourceItem} title={sourceNameDictionary[s.id]}>{s.id}<div className={styles.goOutLink} title={s.url}></div></span>)}
                                    </div>
                                    <div className={styles.sourceBlock}>
                                        <span className={styles.sourceText}>Last date:</span>
                                        <span className={styles.sourceItem}>{new Date(Date.parse(ee.lastGeometryDate || '')).toLocaleDateString()}</span>
                                    </div>
                                    <div className={styles.statusBlock}>
                                        <span className={styles.sourceText}>Status:</span>
                                        <span className={ee.status == EonetEventStatus.Open ? styles.statusOpen : styles.statusClosed}>
                                            {ee.status == EonetEventStatus.Open ? 'Open' : `Closed at ${new Date(Date.parse(ee.closed || '')).toLocaleDateString()}`}
                                        </span>
                                    </div>
                                </div>
                            </Link>
                        </div>
                    ))}
                </List>
            </div>
        </div>
    );
}
