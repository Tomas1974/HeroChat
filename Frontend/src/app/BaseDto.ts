import {messageModel} from "./Data/messageModel";

export class BaseDto<T> {
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}



export class ServerSendStoredMessageToClientDto extends BaseDto<ServerSendStoredMessageToClientDto> {

  message?: string[];
  from?: string[];
  roomId?: string[];

}




export class newMessageToStoreDto extends BaseDto<newMessageToStoreDto> {

  message?: string;
  from?:string;
  roomId?:string;

}


