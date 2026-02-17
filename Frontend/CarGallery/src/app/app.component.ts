import { Component } from '@angular/core';
import { AuthStateService } from "./services/auth-state/auth-state.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'CarGallery';
  currentYear = new Date().getFullYear();
  constructor(public authState: AuthStateService) {}
  ngOnInit() {
    this.authState.ensureLoaded().subscribe();
  }
}
