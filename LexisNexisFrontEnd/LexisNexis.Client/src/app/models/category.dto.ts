export interface CategoryDto {
  id: number;
  name: string;
  description: string;
  parentCategoryId?: number;
  createdAt: string;
  updatedAt: string;
}
