import { Injectable } from '@angular/core';
import {Idata} from "./DataInterface/idata";
import {messageModel} from "./Mock_data/messageModel";
import {BaseDto} from "../BaseDto";
import {MessageService} from "../message.service";

@Injectable({
  providedIn: 'root'
})
export class BackendDataService implements Idata
{

  serviceMessageArray: messageModel[]=[];


  ws: WebSocket = new WebSocket("ws://localhost:8181")

  constructor(public messageService: MessageService) {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);

    }
  }


  getMessages(): void {

    var object = {
      eventType: "ClientWantsToSignIn",
      Username: this.messageService.selectedHero,
      roomId: this.messageService.roomNumber
    }
    this.ws.send(JSON.stringify(object));


    var object1 = {
      eventType: "ClientWantsToEnterRoom",
      roomId: this.messageService.roomNumber
    }
    this.ws.send(JSON.stringify(object1));


    var object2 = {
      eventType: "ClientWantsToGetStoredMessagesToRoom",
      roomId: this.messageService.roomNumber
    }
    this.ws.send(JSON.stringify(object2));


}

  saveMessage(message: messageModel): void {
    var object = {
      eventType: "ClientWantsToBroadcastToRoom",
      roomId: this.messageService.roomNumber,
      message: message.ChatMessage

    }
    this.ws.send(JSON.stringify(object));
  }


}
