import { Component, HostBinding, input } from '@angular/core';
import { MatRippleModule } from '@angular/material/core';

@Component({
  selector: 'codelab-button, button[codelabButton]',
  imports: [MatRippleModule],
  templateUrl: './codelab-button.html',
  styleUrl: './codelab-button.scss',
})
export class CodelabButton {
  appareance = input.required<'primary' | 'danger'>();

  @HostBinding('class.primary')
  get isPrimary() {
    return this.appareance() === 'primary';
  }

  @HostBinding('class.danger')
  get isDanger() {
    return this.appareance() === 'danger';
  }
}
