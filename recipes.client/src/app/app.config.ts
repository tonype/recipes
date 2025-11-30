import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideIcons } from '@ng-icons/core';
import { heroPlus, heroChevronUp, heroChevronDown } from '@ng-icons/heroicons/outline';
import { matRefresh } from '@ng-icons/material-icons/baseline';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    provideIcons({ heroPlus, heroChevronUp, heroChevronDown, matRefresh })
  ]
};
