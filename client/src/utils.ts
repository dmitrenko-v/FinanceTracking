/* eslint-disable */
export function handleError(error: any, errorsFromBackend: string[]) {
  try {
    errorsFromBackend = JSON.parse(error.error.detail).map(
      (e: { Code: string; Description: string }) => e.Description,
    );
  } catch {
    errorsFromBackend.push(error.error.detail);
  }
}
