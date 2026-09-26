import { AfterViewInit, Component, Input, OnDestroy } from '@angular/core';
import * as L from 'leaflet';
import { Stop } from '../../core/models/stop.model';


delete (L.Icon.Default.prototype as any)._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'leaflet/marker-icon-2x.png',
  iconUrl: 'leaflet/marker-icon.png',
  shadowUrl: 'leaflet/marker-shadow.png',
});

@Component({
  selector: 'app-trip-map',
  standalone: true,
  template: `<div id="trip-map" style="height: 320px; border-radius: 12px;"></div>`
})
export class TripMapComponent implements AfterViewInit, OnDestroy {
  @Input() stops: Stop[] = [];
  private map?: L.Map;

  ngAfterViewInit(): void {
    this.map = L.map('trip-map').setView([38.7223, -9.1393], 6); // default: Lisbon

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
      maxZoom: 19
    }).addTo(this.map);

    if (this.stops.length > 0) {
      const latLngs = this.stops.map(s => L.latLng(s.latitude, s.longitude));
      const bounds = L.latLngBounds(latLngs);

      this.stops.forEach((stop, i) => {
        L.marker([stop.latitude, stop.longitude])
          .addTo(this.map!)
          .bindPopup(`${i + 1}. ${stop.name}`);
      });

      L.polyline(latLngs, { color: '#7F77DD', dashArray: '4 4' }).addTo(this.map);
      this.map.fitBounds(bounds, { padding: [30, 30] });
    }
  }

  ngOnDestroy(): void {
    this.map?.remove();
  }
}