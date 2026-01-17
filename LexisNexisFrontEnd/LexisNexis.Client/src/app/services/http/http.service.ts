import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment';
import { IHttpService } from './http.interface';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class HttpService implements IHttpService {
  constructor(private http: HttpClient) {}

  private baseUrl = environment.apiBaseUrl;

  private jsonHeaders = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  get<T>(endpoint: string, options: object = {}): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${endpoint}`, options);
  }

  post<T>(endpoint: string, body: any, options: object = {}): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${endpoint}`, body, options);
  }

  put<T>(endpoint: string, body: any, options: object = {}): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${endpoint}`, body, options);
  }

  delete<T>(endpoint: string, options: object = {}): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${endpoint}`, options);
  }
}
