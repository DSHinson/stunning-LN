import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';

/**
 * State interface for categories.
 * Contains both flat and tree structures, along with loading and error info.
 */
export interface CategoryState {
  /** Flat list of categories from the API */
  flat: CategoryDto[];

  /** Tree structure of categories from the API */
  tree: CategoryTreeDto[];

  /** Loading flags */
  loadingFlat: boolean;
  loadingTree: boolean;

  /** Optional error messages */
  errorFlat?: string;
  errorTree?: string;
}

/**
 * Initial state for the Category store
 */
export const initialCategoryState: CategoryState = {
  flat: [],
  tree: [],
  loadingFlat: false,
  loadingTree: false,
  errorFlat: undefined,
  errorTree: undefined
};
