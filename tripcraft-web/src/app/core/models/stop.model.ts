export interface Stop {
  id: string;
  tripId: string;
  name: string;
  address: string | null;
  latitude: number;
  longitude: number;
  order: number;
}

export interface CreateStopRequest {
  name: string;
  address?: string;
  latitude: number;
  longitude: number;
  order: number;
}