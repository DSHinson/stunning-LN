import { ProductDto } from '../../models/product.dto';

/**
 * Represents all UI-relevant state for the Product feature.
 * This is the single source of truth for product-related data.
 */
export interface ProductState {
  products: ProductDto[];
  loading: boolean;
  error?: string;
}


/**
 * Initial state used when the store is first created.
 */
export const initialProductState: ProductState = {
  products: [],
  loading: false
};
