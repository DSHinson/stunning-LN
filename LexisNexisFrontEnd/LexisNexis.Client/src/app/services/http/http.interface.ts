import { Observable } from "rxjs";

export interface IHttpService {
  get<T>(endpoint: string, options?: object): Observable<T>;
  post<T>(endpoint: string, body: any, options?: object): Observable<T>;
  put<T>(endpoint: string, body: any, options?: object): Observable<T>;
  delete<T>(endpoint: string, options?: object): Observable<T>;
}
