import { Injectable } from '@angular/core';
import {roomNumberArray} from "./Data/roomNumberArray";
import {messageModel, roomModel} from "./Data/messageModel";

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor() { }

  roomNumberArray: roomModel[]=roomNumberArray;


  getRoomNumber(user1: string, user2: string) : number
  {

    let roommodel: roomModel | undefined = this.roomNumberArray.find(number =>
      (number.from === user1 && number.to === user2) || (number.from === user2 && number.to === user1));

  return <number>roommodel?.room;

  }



}
