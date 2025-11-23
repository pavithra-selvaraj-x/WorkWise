import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'ww-page-title-bar',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './page-title-bar.component.html',
  styleUrl: './page-title-bar.component.scss',
})
export class PageTitleBarComponent {
  @Input() pageTitle: string = '';
  @Input() buttonText: string = '';
  @Input() iconName: string = '';
  @Input() showClose: boolean = false;

  @Output() buttonPressed = new EventEmitter();
  @Output() closePressed = new EventEmitter();

  onButtonPress() {
    this.buttonPressed.emit();
  }

  onClosePress() {
    this.closePressed.emit();
  }
}
