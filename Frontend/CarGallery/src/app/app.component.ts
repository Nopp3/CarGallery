import { Component } from '@angular/core';
import { SessionService } from "./services/session/session.service";
import { SharedService } from "./services/shared/shared.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'CarGallery';
  userLogged = SessionService.get("LoggedUser")
  constructor(private sharedService: SharedService) {}
  ngOnInit() {
    this.sharedService.refreshEvent.subscribe(() => {
      this.userLogged = SessionService.get("LoggedUser")
    });
  }
}
