import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  refreshEvent: EventEmitter<void> = new EventEmitter<void>();
  emitRefreshEvent() {
    this.refreshEvent.emit();
  }
}
