import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';

@Pipe({
  name: 'dates'
})
export class DatesPipe implements PipeTransform {

  transform(value: any): any {
    return formatDate(value, 'dd/MM/yyyy', 'en-US');
  }

}