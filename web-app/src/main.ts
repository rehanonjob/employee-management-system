import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';

import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';

bootstrapApplication(App, {
  ...appConfig,
  providers: [
    provideAnimations(),   
    provideToastr(),       
    ...(appConfig.providers || [])  
  ]
}).catch(err => console.error(err));
