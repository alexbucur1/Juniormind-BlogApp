import {
  Component, Input, ViewChild,
} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent {
  @Input() message!: string;
  @Input() type!: string;

  @ViewChild('content', { static: false }) private content!: { nativeElement: string; };

  constructor(private modalService: NgbModal) { }

  async open() {
    let submited = false;
    await this.modalService.open(this.content).result.then(() => {
      submited = true;
    }, () => {
    });

    return submited;
  }
}
