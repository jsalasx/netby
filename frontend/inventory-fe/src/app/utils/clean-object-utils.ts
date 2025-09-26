export class ObjectUtils {
  static cleanObject<T extends object>(obj: T): Partial<T> {
    return Object.fromEntries(
      Object.entries(obj).filter(([_, v]) => v !== null && v !== undefined && v !== '')
    ) as Partial<T>;
  }
}
