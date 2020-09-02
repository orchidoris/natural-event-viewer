import React, { useState } from 'react';
import GoogleMapReact from 'google-map-react';
import * as styles from './Map.module.css';
import Marker from './Marker';

const AnyReactComponent = ({text}: any) => <div>{text}</div>;

export type PointProps = {
  id: string,
  isClosed: boolean;
  lat: number,
  lng: number,
  text: string,
  categoryClass: string,
  categoryId: string
}

export type Props = {
  points: PointProps[],
  zoom: number
}

const Map = (props: Props) => {
    const [center, setCenter] = useState({lat: props.points.length > 1 ? 0 : props.points[0].lat, lng: props.points.length > 1 ? 0 : props.points[0].lng });
    const [zoom, setZoom] = useState(props.zoom);

    return (
        <div className={styles.mapContainer} style={{ height: props.points.length > 1 ? '100%' : '100vh', width: '100%' }}>
        <GoogleMapReact
          bootstrapURLKeys={{ key: 'AIzaSyAQuBq-dAioqVBPvkolDO6kHqhkl7s8o_E' }}
          defaultCenter={center}
          defaultZoom={zoom}
        >
          {props.points.map(p => (
            <Marker
              id={p.id}
              isClosed={p.isClosed}
              lat={p.lat}
              key={p.id}
              lng={p.lng}
              text={p.text}
              categoryClass={p.categoryClass}
              categoryId={p.categoryId}
            />
          ))}
        </GoogleMapReact>
      </div>
    );
}

export default Map;