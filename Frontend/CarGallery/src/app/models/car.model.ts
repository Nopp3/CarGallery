export interface CarUI {
  id: string, //GUID
  username: string,
  fuel: string,
  body: string,
  brand: string,
  model:	string,
  productionYear: number,
  engine: number,
  horsePower: number,
  imagePath: string
}

export interface Car {
  id: string, //GUID
  user_id: string, //GUID
  fuel_id: number,
  body_id: number,
  brand_id: number,
  model:	string,
  productionYear: number,
  engine: number,
  horsePower: number,
  imagePath: string
}

export interface Body {
 id: number,
 type: string
}

export interface Brand {
  id: number,
  name: string
}

export interface Fuel {
  id: number,
  type: string
}
