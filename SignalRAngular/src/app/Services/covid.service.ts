import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { CovidModel } from '../Models/covid.model';

@Injectable({
  providedIn: 'root'
})
export class CovidService {
  private connection!: signalR.HubConnection;

  private startInvoke(){
    this.connection.invoke('GetCovidList').catch((err) => {
      console.log(err);
    });
  }
  
  
  constructor() { }

  startConnection(){
    this.connection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5011/CovidHub').build();

    this.connection.start().then(() => {
      this.startInvoke();
    }).catch((err) => {
      console.log(err);
    });
  }
  
  covidChartList = new Array();

  startListener(){
    this.connection.on('ReceiveCovidList', (covidCharts:CovidModel[]) => {
      this.covidChartList=[];
      covidCharts.forEach((item) => {
        this.covidChartList.push([item.covidDate, 
          item.counts[0],
          item.counts[1],
          item.counts[2],
          item.counts[3],
          item.counts[4]
        ]);
      });
    });
  }
}