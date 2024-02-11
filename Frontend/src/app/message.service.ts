import { Injectable } from '@angular/core';
import {message_mock} from "./Data/message_mock";
import {messageModel, roomModel} from "./Data/messageModel";
import {roomNumberArray_mock} from "./Data/roomNumberArray";
import {BaseDto, newMessageToStoreDto, ServerSendStoredMessageToClientDto} from "./BaseDto";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  //serviceMessageArray = message_mock;
  serviceMessageArray: messageModel[] = [];
  selectedMessageArray: messageModel[] = [];
  roomNumberArray: roomModel[] = roomNumberArray_mock;
  superHeroes: string[] = ["Superman", "Spiderman", "Iron Man", "Batman", "Captain America"];
  selectedHero: string = "";
  roomNumber:number=0;
  selectedToMessageTo: string="";
  messageArray: string[]=[];


  ws: WebSocket = new WebSocket("ws://localhost:8181")

  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);

    }
  }


  filterMessagesByFromAndTo(): string[] {




    this.selectedMessageArray = this.serviceMessageArray.filter(mes => mes.room == this.roomNumber);

    const messageString = this.selectedMessageArray.map(message => `[${message.ChatFrom}]: ${message.ChatMessage}`);

    return messageString;
  }

  saveMessage(messageModel: messageModel) {
    this.serviceMessageArray.push(messageModel);


      var object = {
        eventType: "ClientWantsToBroadcastToRoom",
        message: messageModel.ChatMessage,
        roomId: this.roomNumber
      }
      this.ws.send(JSON.stringify(object));

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

    var object1 = {
      eventType: "ClientWantsToGetStoredMessagesToRoom",
      roomId: roomNumber
    }
    this.ws.send(JSON.stringify(object1));



  }

  ServerSendStoredMessageToClient(dto:ServerSendStoredMessageToClientDto)
  {
    this.serviceMessageArray=[];

      // @ts-ignore
    if (dto.message.length>0)
    {
// @ts-ignore
      for (let i = 0; i < dto.message.length; i++) {
        let messageModel:messageModel={
          // @ts-ignore
          room: dto.roomId[i],
          // @ts-ignore
          ChatMessage: dto.message[i],
          // @ts-ignore
          ChatFrom: dto.from[i]};


        this.serviceMessageArray.push(messageModel);
      }
      console.log(this.serviceMessageArray.length)

      this.roomNumber=this.getRoomNumber(this.selectedHero, this.selectedToMessageTo)
      this.messageArray=this.filterMessagesByFromAndTo();
    }

  }

  newMessageToStore(dto:newMessageToStoreDto)
  {

    // @ts-ignore
    const messageModel:messageModel={room: dto.roomId, ChatMessage: dto.message, ChatFrom:dto.username};
    this.serviceMessageArray.push(messageModel);
  }


}






