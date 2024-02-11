import {Component, OnInit} from '@angular/core';
import {MessageService} from "./message.service";
import {messageModel} from "./Data/messageModel";


@Component({
  selector: 'app-root',
  template: `

  <ion-app >

    <ion-col>
    <ion-row>
    <ion-header style="font-size: 30px;"> Superhero Chat</ion-header>
    </ion-row>
    <ion-row>

        <ion-col size="4">
      <ion-row>


        <ion-title>Superhero Name</ion-title>
      </ion-row>

        <ion-row>
        <ion-list>
        <ion-item>
            <ion-select placeholder="Super Name" [(ngModel)]="messageService.selectedHero" (ionChange)="getHeroesToChat()">
              <div slot="label">
                <ion-text >Name</ion-text>
              </div>

              <ion-select-option *ngFor="let text of superHeroes" [value]="text">{{ text }}</ion-select-option>
            </ion-select>
          </ion-item>
        </ion-list>



      </ion-row>
          <ion-row>

            <ion-item>
            <ion-input>Password</ion-input>
            </ion-item>

          </ion-row>

          <ion-row>
            <ion-button>Login</ion-button>
            <ion-button>Log out</ion-button>

          </ion-row>



        </ion-col>


      <ion-col size="5">


        <ion-row>


          <ion-title>Chat to:</ion-title>

        </ion-row>

        <ion-row>
          <ion-list>
            <ion-item>

              <ion-select placeholder="Super name1" [(ngModel)]="this.messageService.selectedToMessageTo" (ionChange)="getChatMessages()">
                <ion-text >Name</ion-text>
                <ion-select-option *ngFor="let text of chatToHeroes" [value]="text">{{ text }}</ion-select-option>
              </ion-select>


            </ion-item>
          </ion-list>

        </ion-row>

          <ion-item>

            <ion-textarea style="width: 100%; min-height: 25em; border: 1px solid #000;"  [readonly]="true" [value]="concatenatedText"></ion-textarea>

          </ion-item>

          <ion-item>
            <ion-textarea style="width: 100%; min-height: 5em; border: 1px solid #000;"[(ngModel)]="message" label="Message" placeholder="Type something here">
             </ion-textarea>
          </ion-item>

        <ion-item>
          <ion-button style="display: flex; justify-content: flex-end;" (click)="sendMessage()">Send message</ion-button>
        </ion-item>


      </ion-col>
      <ion-col>


      </ion-col>


    </ion-row>


    </ion-col>
     </ion-app>


  `,
  styleUrls: ['app.component.scss'],
})
export class AppComponent {

  superHeroes: string[]=this.messageService.superHeroes;
  chatToHeroes: string[]=[];


  message: string="";





  constructor(public messageService: MessageService ) {

  }

  sendMessage() {


    this.messageService.messageArray.push("["+this.messageService.selectedHero+"]: "+this.message);

    const mesageModel:messageModel={room: this.messageService.roomNumber, ChatMessage: this.message, ChatFrom:this.messageService.selectedHero};
    this.messageService.saveMessage(mesageModel);

    this.message="";

  }


  get concatenatedText(): string {


    return this.messageService.messageArray.join('\n');

  }


  getChatMessages() {

  this.messageService.getMessages(this.messageService.roomNumber);


  }

  getHeroesToChat()
  {
    this.chatToHeroes=this.superHeroes.filter(heroes => heroes!=this.messageService.selectedHero);
   this.messageService.messageArray=[];
    this.messageService.selectedToMessageTo="";
    this.messageService.sendHero();
  }





}
