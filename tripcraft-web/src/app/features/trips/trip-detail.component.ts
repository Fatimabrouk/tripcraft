import { Component } from '@angular/core';
import { TripMapComponent } from './trip-map.component';
import { Stop } from '../../core/models/stop.model';

@Component({
  selector: 'app-trip-detail',
  standalone: true,
  imports: [TripMapComponent],
  template: `
    <h2>Trip Detail</h2>
    <app-trip-map [stops]="sampleStops"></app-trip-map>
  `
})
export class TripDetailComponent {
  sampleStops: Stop[] = [
    { id: '1', tripId: 't1', name: 'Belem', address: '', latitude: 38.6979, longitude: -9.2062, order: 1 },
    { id: '2', tripId: 't1', name: 'Alfama', address: '', latitude: 38.7139, longitude: -9.1315, order: 2 },
    { id: '3', tripId: 't1', name: 'Porto', address: '', latitude: 41.1579, longitude: -8.6291, order: 3 }
  ];
}