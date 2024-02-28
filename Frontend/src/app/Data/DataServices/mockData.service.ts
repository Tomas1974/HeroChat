import { Injectable } from '@angular/core';
import {message_mock} from "../Mock_data/message_mock";
import {messageModel, roomModel} from "../Mock_data/messageModel";
import {roomNumberArray_mock} from "../Mock_data/roomNumberArray";
import {BaseDto, newMessageToStoreDto, ServerSendStoredMessageToClientDto} from "../../BaseDto";
import {Idata} from "./idata";


@Injectable({
  providedIn: 'root'
})
export class BackendDataService implements Idata{

  serviceMessageArray = message_mock; //Use this if Mockmode is true:
    roomNumberArray: roomModel[] = roomNumberArray_mock;

  selectedMessageArray: messageModel[] = [];
  superHeroes: string[] = ["Superman", "Spiderman", "Iron Man", "Batman", "Captain America"];
  selectedHero: string = "";
  roomNumber:number=0;
  selectedToMessageTo: string="";
  messageArray: string[]=[];




  constructor() {  }


  saveMessage(messageModel: messageModel) {

    this.serviceMessageArray.push(messageModel);

  }



  getMessages()
  {
    this.roomNumber=this.getRoomNumber(this.selectedHero, this.selectedToMessageTo)
    this.messageArray=this.filterMessagesByFromAndTo();

  }


  filterMessagesByFromAndTo(): string[] {

    this.selectedMessageArray = this.serviceMessageArray.filter(mes => mes.room == this.roomNumber);

    const messageString = this.selectedMessageArray.map(message => `[${message.ChatFrom}]: ${message.ChatMessage}`);

    return messageString;
  }


  getRoomNumber(user1: string, user2: string): number {

    let roommodel: roomModel | undefined = this.roomNumberArray.find(number =>
      (number.from === user1 && number.to === user2) || (number.from === user2 && number.to === user1));

    return <number>roommodel?.room;

  }


}






