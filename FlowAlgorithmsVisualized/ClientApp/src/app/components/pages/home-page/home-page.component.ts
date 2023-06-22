import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../../../services/navigation.service';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  isExpanded = false;
  opened = false;

  constructor(private navigationService: NavigationService) {
    this.navigationService.getJSON().subscribe(data => {
      console.log(data);
      this.dataSource.data = data;
      this.treeControl.expandAll();
    });
  }

  ngOnInit() {
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  private _transformer = (node: MenuNode, level: number) => {
    return {
      id: node.id,
      name: node.name,
      route: node.route,
      expandable: !!node.children && node.children.length > 0,
      level: level,
    };
  };

  treeControl = new FlatTreeControl<Node>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children,
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: Node) => node.expandable;

}

interface MenuNode {
  id?: number;
  name: string;
  route?: string;
  children?: MenuNode[];
}

interface Node {
  id?: number;
  name: string;
  route?: string;
  level: number;
  expandable: boolean;
}
