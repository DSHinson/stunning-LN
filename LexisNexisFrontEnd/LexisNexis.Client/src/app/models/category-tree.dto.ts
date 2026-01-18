export interface CategoryTreeDto {
  id: number;
  name: string;
  description?: string;
  children?: CategoryTreeDto[];
}
