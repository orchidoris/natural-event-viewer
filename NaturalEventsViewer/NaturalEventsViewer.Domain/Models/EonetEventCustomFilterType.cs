namespace NaturalEventsViewer.Domain.Models
{
    public enum EonetEventCustomFilterType
    {
        // no need for custom SourcesSubset as sources can be filtered on API level
        // the need in duplicating API filters may appear if we cache Events list endpoint results
        TitleContains,
        DescripitonContains,
        CategoriesSubset        
    }
}
