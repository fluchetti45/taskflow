import { Status } from './status.model';

export interface Task {
  title: string;
  description: string;
  id: number;
  status: Status;
  statusId: number;
  createdAt: Date;
  userId: number;
  statusName: string;
}
