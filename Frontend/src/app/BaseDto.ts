export class BaseDto<T> {
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}


export class ServerEchosClientDto extends BaseDto<ServerEchosClientDto> {
  echoValue?: string;
}



export class ServerAddsClientToRoom extends BaseDto<ServerAddsClientToRoom> {
  message?: string[];
}

export class ServerBroadcastsMessageWithUsername extends BaseDto<ServerBroadcastsMessageWithUsername> {
  message?: string[];
  username?: string[];
}

