import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
	private selectedGroupId = new BehaviorSubject(0);
	currentGroup = this.selectedGroupId.asObservable();
	constructor() { }

	setGroupId(groupId: number) {
		this.selectedGroupId.next(groupId);
	}
}
