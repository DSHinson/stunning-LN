import { InjectionToken } from "@angular/core";
import { IHttpService } from "./http.interface";

export const HTTP_SERVICE = new InjectionToken<IHttpService>('HttpService', {
  providedIn: 'root',
   factory: () => {
    // Default implementation (will be overridden in providers)
    throw new Error('HTTP_SERVICE not provided');
  }
});
