export interface AddGoalDto {
  title: string;
  description: string;
  goalAmount: number;
  currentAmount: number;
  storedIn: string;
  deadline: Date;
}
