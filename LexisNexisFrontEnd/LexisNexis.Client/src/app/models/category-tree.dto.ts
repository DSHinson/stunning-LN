export interface CategoryTreeDto {
  id: number;
  name: string;
  description?: string;
  Nodes?: CategoryTreeDto[];
}
