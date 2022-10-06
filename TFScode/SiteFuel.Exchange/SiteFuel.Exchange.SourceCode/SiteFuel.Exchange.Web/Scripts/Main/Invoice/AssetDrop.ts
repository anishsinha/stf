module AssetDropModule {
    declare var CollectionName: string;

    export class AssetDrop {
        Id: number;
        OrderId: number;
        AssetName: string;
        JobXAssetId: number;
        MeterStartReading: number;
        MeterEndReading: number;
        DropGallons: number;
        StartTime: string;
        EndTime: string;
        IsActive: boolean;
        Index: number;
        UoM: string;
        PreDip: number;
        PostDip: number;
        TankScaleMeasurement: number;
        AssetType: number;

        constructor(data, index: number, uom: string) {
            this.Id = data.Id;
            this.OrderId = data.OrderId;
            this.AssetName = data.AssetName;
            this.JobXAssetId = data.JobXAssetId;
            this.MeterStartReading = data.MeterStartReading;
            this.MeterEndReading = data.MeterEndReading;
            this.DropGallons = data.DropGallons;
            this.StartTime = data.StartTime;
            this.EndTime = data.EndTime;
            this.IsActive = data.IsActive;
            this.Index = index;
            this.UoM = uom;
            this.PreDip = data.PreDip;
            this.PostDip = data.PostDip;
            this.TankScaleMeasurement = data.TankScaleMeasurement;
        }

        private getIdElement() {
            var html = '<input id="' + CollectionName + '_' + this.Index + '_Id" name="' + CollectionName + '[' + this.Index + '].Id" type="hidden" value="' + this.Id + '">';
            return html;
        }
        private getOrderIdElement() {
            var html = '<input id="' + CollectionName + '_' + this.Index + '_OrderId" name="' + CollectionName + '[' + this.Index + '].OrderId" type="hidden" value="' + this.OrderId + '">';
            return html;
        }
        private getAssetNameElement() {
            var StrId = CollectionName + '_' + this.Index + '_AssetName';
            var StrName = CollectionName + '[' + this.Index + '].AssetName';
            var html = '<label class="f-normal asset-name" for="' + StrId + '">Name</label>';
            html += '<input class="form-control" id="' + StrId + '" name="' + StrName + '" type="text" value="' + this.AssetName + '" data-val="true" data-val-required="Name is required" readonly="true">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }
        private getJobXAssetIdElement() {
            var html = '<input id="' + CollectionName + '_' + this.Index + '_JobXAssetId" name="' + CollectionName + '[' + this.Index + '].JobXAssetId" type="hidden" value="' + this.JobXAssetId + '">';
            return html;
        }
        private getMeterStartReadingElement() {
            var html = '<input id="' + CollectionName + '_' + this.Index + '_MeterStartReading" name="' + CollectionName + '[' + this.Index + '].MeterStartReading" type="hidden" value="' + this.MeterStartReading + '">';
            return html;
        }
        private getMeterEndReadingElement() {
            var html = '<input id="' + CollectionName + '_' + this.Index + '_MeterEndReading" name="' + CollectionName + '[' + this.Index + '].MeterEndReading" type="hidden" value="' + this.MeterEndReading + '">';
            return html;
        }
        private getDropGallonsElement() {
            var StrId = CollectionName + '_' + this.Index + '_DropGallons';
            var StrName = CollectionName + '[' + this.Index + '].DropGallons';
            var html = '<label class="f-normal" for="' + StrId + '">'+ this.UoM +'</label>';
            html += '<input class="form-control drop-input" id="' + StrId + '" name="' + StrName + '" type="text" value="' + (this.DropGallons == null ? '' : this.DropGallons) + '" data-val="true" data-val-number="The field DropGallons must be a number.">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }
        private getPreDipElement() {
            var StrId = CollectionName + '_' + this.Index + '_PreDip';
            var StrName = CollectionName + '[' + this.Index + '].PreDip';
            var html = '<label class="f-normal" for="' + StrId + '">Pre Dip</label>';
            html += '<input class="predip form-control" id="' + StrId + '" name="' + StrName + '" type="text" value="' + (this.PreDip == null ? '' : this.PreDip) + '">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }
        private getPostDipElement() {
            var StrId = CollectionName + '_' + this.Index + '_PostDip';
            var StrName = CollectionName + '[' + this.Index + '].PostDip';
            var html = '<label class="f-normal" for="' + StrId + '">Post Dip</label>';
            html += '<input class="postdip form-control" id="' + StrId + '" name="' + StrName + '" type="text" value="' + (this.PostDip == null ? '' : this.PostDip) + '">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }

        private getUoMdropdownElement() {
            var StrId = CollectionName + '_' + this.Index + '_TankScaleMeasurement';
            var StrName = CollectionName + '[' + this.Index + '].TankScaleMeasurement';
            var html = '<label class="f-normal" for="' + StrId + '">UoM</label>';
            html += '<select class="form-control tankScale" id="' + StrId + '" name="' + StrName + '">';
            html += '<option value="" selected disabled> Select UOM </option>';
            html += '<option value = "1">Cm</option>';
            html += '<option value = "2">in</option>';
            html += '<option value = "3">Gallons</option>';
            html += '<option value = "4">Litres</option>';
            html += '</select>';
        }
        private getStartTimeElement() {
            var StrId = CollectionName + '_' + this.Index + '_StartTime';
            var StrName = CollectionName + '[' + this.Index + '].StartTime';
            var html = '<label class="f-normal" for="' + StrId + '">Start Time</label>';
            html += '<input class="starttime form-control timepicker-withseconds" id="' + StrId + '" name="' + StrName + '" type="text" value="' + (this.StartTime == null ? '' : this.StartTime) + '" data-val="true" data-val-requiredifnotempty="Start Time is required" data-val-requiredifnotempty-dependentproperty="DropGallons">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }
        private getEndTimeElement() {
            var StrId = CollectionName + '_' + this.Index + '_EndTime';
            var StrName = CollectionName + '[' + this.Index + '].EndTime';
            var html = '<label class="f-normal" for="' + StrId + '">End Time</label>';
            html += '<input class="endtime form-control timepicker-withseconds" id="' + StrId + '" name="' + StrName + '" type="text" value="' + (this.EndTime == null ? '' : this.EndTime) + '" data-val="true" data-val-requiredifnotempty="End Time is required" data-val-requiredifnotempty-dependentproperty="DropGallons">';
            html += '<span class="field-validation-valid" data-valmsg-for="' + StrName + '" data-valmsg-replace="true"></span>';
            return html;
        }
        private getTrashElement() {
            return '<a href="javascript:void(0)" class="fa fa-trash-alt color-maroon mt7" title="Remove" onclick="removePartialAndUpdate(this);"></a>';
        }
        private getFirstColumn() {
            var html = '<div class="col-sm-2 col-xs-12 mb15 break-word">';
            html += this.getIdElement();
            html += this.getOrderIdElement();
            html += this.getJobXAssetIdElement();
            html += this.getMeterStartReadingElement();
            html += this.getMeterEndReadingElement();
            html += this.getAssetNameElement();
            html += '</div>';
            return html;
        }
        private getSecondColumn() {
            var html = '<div class="col-sm-2 col-xs-12 mb15">';
            html += this.getDropGallonsElement();
            html += '</div>';
            return html;
        }
        private getThirdColumn() {

            var html = '<div class="col-md-5 col-sm-6"><div class="row">';
            html += '<div class="col-sm-3"><div class="form-group mb5">' + this.getStartTimeElement() + '</div></div>';
            html += '<div class="col-sm-3"><div class="form-group mb5">' + this.getEndTimeElement() + '</div></div>';
            html += '<div class="col-sm-3"><div class="form-group mb5">' + this.getPreDipElement() + '</div></div>';
            html += '<div class="col-sm-3"><div class="form-group mb5">' + this.getPostDipElement() + '</div></div>';
            html += '<div class="col-sm-3"><div class="form-group mb5">' + this.getUoMdropdownElement() + '</div></div>';
            html += '</div></div>';
            return html;
        }
        private getFourthColumn() {
            var html = '<div class="col-sm-1 fs18 mt25 mb10">';
            html += this.getTrashElement();
            html += '</div>';
            return html;
        }
        public getPartialBlock() {
            var html = '<input type="hidden" name="' + CollectionName + '.index" autocomplete="off" value="' + this.Index + '">';
            html += '<div class="row partial-block">';
            html += this.getFirstColumn();
            html += this.getSecondColumn();
            html += this.getThirdColumn();
            html += this.getFourthColumn();
            html += '</div>';
            return html;
        }
    };
}