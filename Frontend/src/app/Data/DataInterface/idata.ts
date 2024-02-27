import { Injectable } from '@angular/core';
import {messageModel} from "../Mock_data/messageModel";



export interface Idata {

  serviceMessageArray: messageModel[];

  saveMessage(message: messageModel): void;

  getMessages():void;

}
