﻿/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// [START compute_firewall_delete]

using Google.Cloud.Compute.V1;
using System.Threading.Tasks;

public class DeleteFirewallRuleAsyncSample
{
    public async Task DeleteFirewallRuleAsync(
        // TODO(developer): Set your own default values for these parameters or pass different values when calling this method.
        string projectId = "your-project-id",
        string firewallRuleName = "my-test-firewall-rule")
    {

        // Initialize client that will be used to send requests. This client only needs to be created
        // once, and can be reused for multiple requests.
        FirewallsClient client = await FirewallsClient.CreateAsync();

        // Make the request to delete the firewall rule.
        var firewallRuleDeletion = await client.DeleteAsync(projectId, firewallRuleName);

        // Wait for the operation to complete using client-side polling.
        await firewallRuleDeletion.PollUntilCompletedAsync();
    }
}

// [END compute_firewall_delete]
