import { Component } from '@angular/core';
import { AuthStateService } from "./services/auth-state/auth-state.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'CarGallery';
  constructor(public authState: AuthStateService) {}
  ngOnInit() {
    this.authState.ensureLoaded().subscribe();
  }
}
