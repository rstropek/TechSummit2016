import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public products: any[];

  constructor(private http: Http) {
    http.get("http://technical-summit-dev.azurewebsites.net/api/products")
      .forEach(resp => {
        this.products = resp.json();
      });
  }
}
