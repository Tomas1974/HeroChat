import { Injectable } from '@angular/core';
import {message_mock} from "./Data/message_mock";
import {messageModel, roomModel} from "./Data/messageModel";
import {roomNumberArray_mock} from "./Data/roomNumberArray";
import {BaseDto} from "./BaseDto";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  serviceMessageArray = message_mock;
  selectedMessageArray: messageModel[] = [];
  roomNumberArray: roomModel[] = roomNumberArray_mock;
  superHeroes: string[] = ["Superman", "Spiderman", "Iron Man", "Batman", "Captain America"];
  selectedHero: string = "";


  ws: WebSocket = new WebSocket("ws://localhost:8181")

  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);

    }
  }


  filterMessagesByFromAndTo(room: number): string[] {
    this.selectedMessageArray = this.serviceMessageArray.filter(mes => mes.room == room);

    const messageString = this.selectedMessageArray.map(message => `[${message.ChatFrom}]: ${message.ChatMessage}`);

    return messageString;
  }

  saveMessage(messageModel: messageModel) {
    this.serviceMessageArray.push(messageModel);
  }

  getRoomNumber(user1: string, user2: string): number {

    let roommodel: roomModel | undefined = this.roomNumberArray.find(number =>
      (number.from === user1 && number.to === user2) || (number.from === user2 && number.to === user1));

    return <number>roommodel?.room;

  }

  sendHero() {
    var object = {
      eventType: "ClientWantsToSignIn",
      Username: this.selectedHero
    }
    this.ws.send(JSON.stringify(object));

  }

  getMessages(roomNumber:number)
  {

    var object = {
      eventType: "ClientWantsToEnterRoom",
      roomId: roomNumber
    }
    this.ws.send(JSON.stringify(object));

  }



}






