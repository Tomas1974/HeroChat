import { Injectable } from '@angular/core';
import {Idata} from "./DataInterface/idata";
import {messageModel} from "./Mock_data/messageModel";
import {message_mock} from "./Mock_data/message_mock";
import {MessageService} from "../message.service";


@Injectable({
  providedIn: 'root'
})
export class MockDataService implements Idata{

  constructor(public messageService: MessageService) {
  }

  serviceMessageArray = message_mock;

  getMessages(): void {
    this.messageService.messageArray=this.messageService.filterMessagesByFromAndTo();

  }

  saveMessage(message: messageModel): void {

    this.serviceMessageArray.push(message);

  }
}
