import { TestBed } from '@angular/core/testing';
import { ProductService } from './product.service';
import { HTTP_SERVICE } from '../http/http.token';
import { of } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ProductDto } from '../../models/product.dto';

describe('ProductService', () => {
  let service: ProductService;
  let httpSpy: jasmine.SpyObj<any>;

  beforeEach(() => {
    httpSpy = jasmine.createSpyObj('HTTP_SERVICE', ['get', 'post', 'put', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        ProductService,
        { provide: HTTP_SERVICE, useValue: httpSpy }
      ]
    });

    service = TestBed.inject(ProductService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a product', () => {
    const product: ProductDto = {
      id: 1,
      name: 'Test Product',
      description: 'Test description',
      sku: 'SKU123',
      categoryId: 5,
      price: 100,
      quantity: 10,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    httpSpy.post.and.returnValue(of(product));

    service.createProduct(product).subscribe(result => {
      expect(result).toEqual(product);
    });

    expect(httpSpy.post).toHaveBeenCalledWith('products', product);
  });

  it('should update a product', () => {
    const update: ProductDto = {
      id: 1,
      name: 'Updated',
      description: 'Updated description',
      sku: 'SKU123',
      categoryId: 5,
      price: 120,
      quantity: 15,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    httpSpy.put.and.returnValue(of(update));

    service.updateProduct(1, update).subscribe(result => {
      expect(result).toEqual(update);
    });

    expect(httpSpy.put).toHaveBeenCalledWith('products/1', update);
  });

  it('should delete a product', () => {
    httpSpy.delete.and.returnValue(of(void 0));

    service.deleteProduct(1).subscribe(result => {
      expect(result).toBeUndefined();
    });

    expect(httpSpy.delete).toHaveBeenCalledWith('products/1');
  });

  it('should get products with query params', () => {
    const response: ProductDto[] = [
      {
        id: 1,
        name: 'Test Product',
        description: 'Test description',
        sku: 'SKU123',
        categoryId: 5,
        price: 100,
        quantity: 10,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString()
      }
    ];

    httpSpy.get.and.returnValue(of(response));

    service.getProducts(1, 10, 'test', 5).subscribe(result => {
      expect(result).toEqual(response);
    });

    const args = httpSpy.get.calls.mostRecent().args;
    expect(args[0]).toBe('products');

    const params: HttpParams = args[1].params;
    expect(params.get('page')).toBe('1');
    expect(params.get('pageSize')).toBe('10');
    expect(params.get('search')).toBe('test');
    expect(params.get('category')).toBe('5');
  });

  it('should get product by id', () => {
    const product: ProductDto = {
      id: 1,
      name: 'Test Product',
      description: 'Test description',
      sku: 'SKU123',
      categoryId: 5,
      price: 100,
      quantity: 10,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    };

    httpSpy.get.and.returnValue(of(product));

    service.getProductById(1).subscribe(result => {
      expect(result).toEqual(product);
    });

    expect(httpSpy.get).toHaveBeenCalledWith('products/1');
  });
});
