import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideHttpClient(withFetch()),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    

    // 👇 importa módulos necessários para Toastr
    importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot({
        positionClass: 'toast-center-center',
        closeButton: true,
        timeOut: 0,
        extendedTimeOut: 0,
        tapToDismiss: false
      })
    )
  ]
};
