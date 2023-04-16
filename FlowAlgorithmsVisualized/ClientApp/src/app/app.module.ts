import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomePageComponent } from './components/pages/home-page/home-page.component';
import { NavMenuComponent } from './components/pages/nav-menu/nav-menu.component';
import { AlgorithmStepsComponent } from './components/pages/algorithm-steps/algorithm-steps.component';

import { GenericWithAugmentingPathComponent } from './components/pseudocode/generic-with-augmenting-path/generic-with-augmenting-path.component';
import { FordFulkersonComponent } from './components/pseudocode/ford-fulkerson/ford-fulkerson.component';
import { EdmondsKarpComponent } from './components/pseudocode/edmonds-karp/edmonds-karp.component';
import { AhujaOrlinCapacityScalingComponent } from './components/pseudocode/ahuja-orlin-capacity-scaling/ahuja-orlin-capacity-scaling.component';
import { GabowComponent } from './components/pseudocode/gabow/gabow.component';
import { AhujaOrlinShortestPathComponent } from './components/pseudocode/ahuja-orlin-shortest-path/ahuja-orlin-shortest-path.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule, MatDividerModule, MatProgressSpinnerModule, MatToolbarModule } from '@angular/material';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    NavMenuComponent,
    AlgorithmStepsComponent,
    GenericWithAugmentingPathComponent,
    FordFulkersonComponent,
    EdmondsKarpComponent,
    AhujaOrlinCapacityScalingComponent,
    GabowComponent,
    AhujaOrlinShortestPathComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'algorithm-steps/:algorithm', component: AlgorithmStepsComponent },
      { path: '', component: HomePageComponent, pathMatch: 'full' },
      { path: '**', redirectTo:'' },
    ]),
    BrowserAnimationsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDividerModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
