
import { DropDownItem } from 'src/app/lfv-dashboard/LiftFileModels';

export class CarrierDetailsViewModel {
    constructor() {
        this.AssignedTerminalId = new DropDownItem();
    }
    public AssignedTerminalId: DropDownItem;
    public CarrierName: string;
    public AssignedCarrierId: string;
    public Id: number; //mapping Id
    public CountryId: number;
    public AssignedTerminalIdName: string;
    public TerminalCompanyAliasId: number;
    
}