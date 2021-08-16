export interface Page<T> {
    pageSize : number;
    pageIndex : number;
    hasNextPage : boolean;
    hasPreviousPage : boolean
    items : Array<T>;
}
