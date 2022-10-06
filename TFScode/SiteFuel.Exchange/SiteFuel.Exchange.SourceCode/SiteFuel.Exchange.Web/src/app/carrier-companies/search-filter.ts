import { Pipe, PipeTransform } from '@angular/core';


@Pipe({ name: 'startsWith' })
export class startsWithPipe implements PipeTransform {
    transform(value: any[], term: string): any[] {
        return value.filter((x: any) => x.Carrier != null && x.Carrier != undefined && x.Carrier.Name.toLowerCase().indexOf(term.toLowerCase()) >= 0)
    }
}

@Pipe({ name: 'startsWithJob' })
export class startsWithJobPipe implements PipeTransform {
    transform(value: any[], term: string): any[] {
        return value.filter((x: any) => x.Job.Name != null && x.Job.Name != undefined && x.Job.Name.toLowerCase().indexOf(term.toLowerCase()) >= 0)
    }
}
