import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function dateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const invalid = new Date(control.value).valueOf() > Date.now();
    return invalid ? { invalidDate: { value: control.value } } : null;
  };
}
