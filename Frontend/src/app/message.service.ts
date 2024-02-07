import { Injectable } from '@angular/core';
import {message_mock} from "./Data/message_mock";
import {messageModel, roomModel} from "./Data/messageModel";
import {roomNumberArray_mock} from "./Data/roomNumberArray";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  serviceMessageArray=message_mock;
  selectedMessageArray: messageModel[]=[];
  roomNumberArray: roomModel[]=roomNumberArray_mock;
  superHeroes: string[]=["Superman","Spiderman","Iron Man", "Batman","Captain America"];

  constructor() { }


  filterMessagesByFromAndTo(room: number ): string[] {
   this.selectedMessageArray= this.serviceMessageArray.filter(mes => mes.room==room);

   const messageString = this.selectedMessageArray.map(message => `[${message.ChatFrom}]: ${message.ChatMessage}`);

   return messageString;
  }

  saveMessage(messageModel:messageModel)
  {
    this.serviceMessageArray.push(messageModel);
  }

  getRoomNumber(user1: string, user2: string) : number
  {

    let roommodel: roomModel | undefined = this.roomNumberArray.find(number =>
      (number.from === user1 && number.to === user2) || (number.from === user2 && number.to === user1));

    return <number>roommodel?.room;

  }







}
