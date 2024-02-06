import { Injectable } from '@angular/core';
import {message_mock} from "./Data/message_mock";
import {messageModel} from "./Data/messageModel";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  serviceMessageArray=message_mock;
  selectedMessageArray=message_mock;
  constructor() { }


  filterMessagesByFromAndTo(room: number ): string[] {
   this.selectedMessageArray= this.serviceMessageArray.filter(mes => mes.room==room);

   const messageString = this.selectedMessageArray.map(message => `[${message.from}]: ${message.message}`);

   return messageString;
  }

  saveMessage(messageModel:messageModel)
  {
    this.serviceMessageArray.push(messageModel);
  }







}
