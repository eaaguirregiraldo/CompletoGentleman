import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'flex flex-col items-center justify-center p-8 gap-2',
    '[attr.aria-busy]': 'true',
    '[attr.aria-label]': 'message() || "Cargando"',
    'role': 'status'
  },
  template: `
    <div 
      class="w-12 h-12 border-4 border-muted border-t-primary rounded-full animate-spin" 
      aria-hidden="true"
    ></div>
    @if (message()) {
      <p class="text-muted-foreground text-sm">{{ message() }}</p>
    }
  `,
  styles: []
})
export class LoadingSpinner {
  message = input<string>('');
}
